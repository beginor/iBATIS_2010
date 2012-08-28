using System;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;

using NUnit.Framework;

using IBatisNet.DataMapper.Test.Domain;
//using NUnit.Framework.SyntaxHelpers;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.Generics
{
    /// <summary>
    /// Tests generic list
    /// 
    /// Interface tests
    /// 1) IList&lgt;LineItem&glt (Order.LineItemsGenericList) <--- QueryForList&lgt;LineItem&glt
    /// 2) IList&lgt;LineItem&glt (Order.LineItemsGenericList) <--- QueryForList&lgt;LineItem&glt Lazy load
    /// 3) IList&lgt;LineItem&glt (Order.LineItemsGenericList) <--- QueryForList&lgt;LineItem&glt with listClass = LineItemCollection2
    /// 4) IList&lgt;LineItem&glt (Order.LineItemsGenericList) <--- QueryForList&lgt;LineItem&glt with listClass = LineItemCollection2 lazy

    /// Strongly typed collection tests
    /// 5) LineItemCollection2 (Order.LineItemCollection2) <--- QueryForList&lgt;LineItem&glt with listClass = LineItemCollection2
    /// 6) LineItemCollection2 (Order.LineItemCollection2) <--- QueryForList&lgt;LineItem&glt  with listClass = LineItemCollection2 Lazy load
    /// </summary>
    [TestFixture] 
    public class ResultMapTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "enumeration-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "coupons-init.sql");

        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        { /* ... */ }

        #endregion

        #region Result Map test

        /// <summary>
        /// Coupons
        /// </summary>
        [Test]
        public void TestJIRA243WithGoupBy()
        {
            IList<Coupon> coupons = sqlMap.QueryForList<Coupon>("GetCouponBrand", null);

            Assert.That(coupons.Count, Is.EqualTo(5));
            Assert.That(coupons[0].BrandIds[0], Is.EqualTo(1));
            Assert.That(coupons[0].BrandIds[1], Is.EqualTo(2));
            Assert.That(coupons[0].BrandIds[2], Is.EqualTo(3));
            Assert.That(coupons[1].BrandIds[0], Is.EqualTo(4));
            Assert.That(coupons[1].BrandIds[1], Is.EqualTo(5));
            Assert.That(coupons[2].BrandIds.Count, Is.EqualTo(0));
            Assert.That(coupons[3].BrandIds.Count, Is.EqualTo(0));
            Assert.That(coupons[4].BrandIds[0], Is.EqualTo(6));
        }

        /// <summary>
        /// Coupons
        /// </summary>
        [Test]
        public void Test243WithoutGoupBy()
        {
            IList<Coupon> coupons = sqlMap.QueryForList<Coupon>("GetCoupons", null);

            Assert.That(coupons.Count, Is.EqualTo(5));
            Assert.That(coupons[0].BrandIds[0], Is.EqualTo(1));
            Assert.That(coupons[0].BrandIds[1], Is.EqualTo(2));
            Assert.That(coupons[0].BrandIds[2], Is.EqualTo(3));
            Assert.That(coupons[1].BrandIds[0], Is.EqualTo(4));
            Assert.That(coupons[1].BrandIds[1], Is.EqualTo(5));
            Assert.That(coupons[2].BrandIds.Count, Is.EqualTo(0));
            Assert.That(coupons[3].BrandIds.Count, Is.EqualTo(0));
            Assert.That(coupons[4].BrandIds[0], Is.EqualTo(6));
        }

        /// <summary>
        /// Test generic Ilist  : 
        /// 1) IList&lgt;LineItem&glt (Order.LineItemsGenericList) <--- QueryForList&lgt;LineItem&glt
        /// </summary>
        [Test]
        public void TestGenericList()
        {
            Order order = sqlMap.QueryForObject<Order>("GetOrderWithGenericListLineItem", 1);

            AssertOrder1(order);

            // Check generic IList collection
            Assert.IsNotNull(order.LineItemsGenericList);
            Assert.AreEqual(3, order.LineItemsGenericList.Count);
        }

        /// <summary>
        /// Test generic Ilist with lazy loading : 
        /// 2) IList&lgt;LineItem&glt (Order.LineItemsGenericList) <--- QueryForList&lgt;LineItem&glt Lazy load
        /// </summary>
        [Test]
        public void TestGenericListLazyLoad()
        {
            Order order = sqlMap.QueryForObject<Order>("GetOrderWithGenericLazyLoad", 1);

            AssertOrder1(order);

            // Check generic IList collection
            Assert.IsNotNull(order.LineItemsGenericList);
            Assert.AreEqual(3, order.LineItemsGenericList.Count);
        }

        /// <summary>
        /// Test generic typed generic Collection on generic IList  
        /// 3) IList&lgt;LineItem&glt (Order.LineItemsGenericList) <--- QueryForList&lgt;LineItem&glt with listClass = LineItemCollection2
        /// </summary>
        [Test]
        public void TestGenericCollectionOnIList()
        {
            Order order = sqlMap.QueryForObject<Order>("GetOrderWithGenericLineItemCollection", 1);

            AssertOrder1(order);

            // Check generic collection
            Assert.IsNotNull(order.LineItemsGenericList);
            Assert.AreEqual(3, order.LineItemsGenericList.Count);
            LineItemCollection2 lines = (LineItemCollection2)order.LineItemsGenericList;
        }

        /// <summary>
        /// Test generic IList with lazy typed collection 
        /// 4) IList&lgt;LineItem&glt (Order.LineItemsGenericList) <--- QueryForList&lgt;LineItem&glt with listClass = LineItemCollection2 lazy
        /// </summary>
        [Test]
        public void TestLazyListGenericMapping()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithGenericLineItemsLazy", 1);

            AssertOrder1(order);

            Assert.IsNotNull(order.LineItemsGenericList);
            Assert.AreEqual(3, order.LineItemsGenericList.Count);
            LineItemCollection2 lines = (LineItemCollection2)order.LineItemsGenericList;
        }

        /// <summary>
        /// Test generic typed generic Collection on generic typed generic Collection
        /// 5) LineItemCollection2 (Order.LineItemCollection2) <--- QueryForList&lgt;LineItem&glt with listClass = LineItemCollection2
        /// </summary>
        [Test]
        public void TestTypedCollectionOnTypedCollection()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithGenericTypedLineItemCollection", 1);

            AssertOrder1(order);

            Assert.IsNotNull(order.LineItemsCollection2);
            Assert.AreEqual(3, order.LineItemsCollection2.Count);

            IEnumerator<LineItem> e = ((IEnumerable<LineItem>)order.LineItemsCollection2).GetEnumerator();
            while (e.MoveNext())
            {
                LineItem item = e.Current;
                Assert.IsNotNull(item);
            }
        }

        /// <summary>
        /// Test generic typed generic Collection lazy
        /// 6) LineItemCollection2 (Order.LineItemCollection2) <--- QueryForList&lgt;LineItem&glt  with listClass = LineItemCollection2 Lazy load
        /// </summary>
        [Test]
        public void TestTypedCollectionLazy()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithGenericTypedLineItemCollectionLazy", 1);

            AssertOrder1(order);

            Assert.IsNotNull(order.LineItemsCollection2);
            Assert.AreEqual(3, order.LineItemsCollection2.Count);

            IEnumerator<LineItem> e = ((IEnumerable<LineItem>)order.LineItemsCollection2).GetEnumerator();
            while (e.MoveNext())
            {
                LineItem item = e.Current;
                Assert.IsNotNull(item);
            }
        }

        #endregion
    }
}
