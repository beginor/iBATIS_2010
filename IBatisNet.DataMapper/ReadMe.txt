INTRODUCTION

  Track the project progress on the Sourceforge project page:
  http://sourceforge.net/projects/ibatisnet/


INSTALL

If you only use the Dao API, add a reference to the IBatisNet.DataAccess.dll, 
IBatisNet.Common.dll in your own projects.

If you only use the Dao API with DataMapper API, add a reference to the IBatisNet.DataAccess.dll, 
IBatisNet.DataMapper.dll, IBatisNet.Common.dll in your own projects.

If you only use DataMapper API, add a reference to the IBatisNet.DataMapper.dll, 
IBatisNet.Common.dll in your own projects.

In web context, if you used put the dao.config, the providers.config, sqlmap.config 
at the same level as your web.config.
In a console or windows context, put theses files in the same directory as you exe.

CONTRIBUTORS

  IBatisNet was originally created by Gilles Bayon. 


LICENSE

  This library has been released under the Apache 2.0 Licence. Please see the
  full license text located in the file Documentation/license.txt.


CREDITS

	All credit for original code in Java go to Clinton Begin (http://www.ibatis.com/).
	Some code come from Norm project on http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=6bf91dea-dea3-4949-9602-ea1ea32a22b4
	

	Sébastien Bouchet wrote an article (in french) about Reflection.Emit proxies (simulates the Java way of doing it), 
	which help me to implement LazyLoading.

	Check it out at :
	http://www.dotnetguru.org/articles/dossiers/instrumentation/proxiesdynamiques.htm or
	http://www.gotdotnet.com/Community/Workspaces/Workspace.aspx?id=82c8f1a4-54c0-476e-bdee-3f260da3f8af

