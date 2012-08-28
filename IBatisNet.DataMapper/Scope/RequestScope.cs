
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 476843 $
 * $Date: 2006-11-19 09:07:45 -0700 (Sun, 19 Nov 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

#region Using

using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.DataExchange;
using IBatisNet.DataMapper.MappedStatements;

#endregion

namespace IBatisNet.DataMapper.Scope
{
    /// <summary>
    /// Hold data during the process of a mapped statement.
    /// </summary>
    public class RequestScope : IScope
    {
        #region Fields

        private IStatement _statement = null;
        private ErrorContext _errorContext = null;
        private ParameterMap _parameterMap = null;
        private PreparedStatement _preparedStatement = null;
        private IDbCommand _command = null;
        private Queue _selects = new Queue();
        bool _rowDataFound = false;
        private static long _nextId = 0;
        private long _id = 0;
        private DataExchangeFactory _dataExchangeFactory = null;
        private ISqlMapSession _session = null;
        private IMappedStatement _mappedStatement = null;
        private int _currentResultMapIndex = -1;
        // Used by N+1 Select solution
        // Holds [IResultMap, IDictionary] couple where the IDictionary holds [key, result object]
        private IDictionary  _uniqueKeys = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique keys.
        /// </summary>
        /// <param name="map">The ResultMap.</param>
        /// <returns>
        /// Returns [key, result object] which holds the result objects that have  
        /// already been build during this request with this <see cref="IResultMap"/>
        /// </returns>
        public IDictionary GetUniqueKeys(IResultMap map)
        {
            if (_uniqueKeys == null)
            {
                return null;
            }
            return (IDictionary)_uniqueKeys[map];
        }

        /// <summary>
        /// Sets the unique keys.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="keys">The keys.</param>
        public void SetUniqueKeys(IResultMap map, IDictionary keys)
        {
            if (_uniqueKeys == null)
            {
                _uniqueKeys = new Hashtable();
            }
            _uniqueKeys.Add(map, keys);
        }
        
        /// <summary>
        ///  The current <see cref="IMappedStatement"/>.
        /// </summary>
        public IMappedStatement MappedStatement
        {
            set { _mappedStatement = value; }
            get { return _mappedStatement; }
        }

        /// <summary>
        /// Gets the current <see cref="IStatement"/>.
        /// </summary>
        /// <value>The statement.</value>
        public IStatement Statement
        {
            get { return _statement; }
        }

        /// <summary>
        ///  The current <see cref="ISqlMapSession"/>.
        /// </summary>
        public ISqlMapSession Session
        {
            get { return _session; }
        }

        /// <summary>
        ///  The <see cref="IDbCommand"/> to execute
        /// </summary>
        public IDbCommand IDbCommand
        {
            set { _command = value; }
            get { return _command; }
        }

        /// <summary>
        ///  Indicate if the statement have find data
        /// </summary>
        public bool IsRowDataFound
        {
            set { _rowDataFound = value; }
            get { return _rowDataFound; }
        }

        /// <summary>
        /// The 'select' result property to process after having process the main properties.
        /// </summary>
        public Queue QueueSelect
        {
            get { return _selects; }
            set { _selects = value; }
        }

        /// <summary>
        /// The current <see cref="IResultMap"/> used by this request.
        /// </summary>
        public IResultMap CurrentResultMap
        {
            get { return _statement.ResultsMap[_currentResultMapIndex]; }
        }

        /// <summary>
        /// Moves to the next result map.
        /// </summary>
        /// <returns></returns>
        public bool MoveNextResultMap()
        {
            if (_currentResultMapIndex < _statement.ResultsMap.Count - 1)
            {
                _currentResultMapIndex++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// The <see cref="ParameterMap"/> used by this request.
        /// </summary>
        public ParameterMap ParameterMap
        {
            set { _parameterMap = value; }
            get { return _parameterMap; }
        }

        /// <summary>
        /// The <see cref="PreparedStatement"/> used by this request.
        /// </summary>
        public PreparedStatement PreparedStatement
        {
            get { return _preparedStatement; }
            set { _preparedStatement = value; }
        }


        #endregion

        #region Constructors


        /// <summary>
        /// Initializes a new instance of the <see cref="RequestScope"/> class.
        /// </summary>
        /// <param name="dataExchangeFactory">The data exchange factory.</param>
        /// <param name="session">The session.</param>
        /// <param name="statement">The statement</param>
        public RequestScope(
            DataExchangeFactory dataExchangeFactory,
            ISqlMapSession session,
            IStatement statement
            )
        {
            _errorContext = new ErrorContext();

            _statement = statement;
            _parameterMap = statement.ParameterMap;
            _session = session;
            _dataExchangeFactory = dataExchangeFactory;
            _id = GetNextId();
        }
        #endregion

        #region Method

        /// <summary>
        /// Check if the specify object is equal to the current object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (this == obj) { return true; }
            if (!(obj is RequestScope)) { return false; }

            RequestScope scope = (RequestScope)obj;

            if (_id != scope._id) return false;

            return true;
        }

        /// <summary>
        /// Get the HashCode for this RequestScope
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)(_id ^ (_id >> 32));
        }

        /// <summary>
        /// Method to get a unique ID
        /// </summary>
        /// <returns>The new ID</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static long GetNextId()
        {
            return _nextId++;
        }
        #endregion

        #region IScope Members

        /// <summary>
        /// A factory for DataExchange objects
        /// </summary>
        public DataExchangeFactory DataExchangeFactory
        {
            get { return _dataExchangeFactory; }
        }

        /// <summary>
        ///  Get the request's error context
        /// </summary>
        public ErrorContext ErrorContext
        {
            get { return _errorContext; }
        }
        #endregion
    }
}
