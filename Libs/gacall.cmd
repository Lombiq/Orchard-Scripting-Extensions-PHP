PATH = %PATH%;"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools"

gacutil -f -i PhpNetCore.dll
gacutil -f -i PhpNetClassLibrary.dll

gacutil -f -i php4ts.dll
gacutil -f -i php5ts.dll

gacutil -f -i PhpNetXmlDom.dll
gacutil -f -i PhpNetMsSql.dll
gacutil -f -i PhpNetPDO.dll
gacutil -f -i PhpNetPDOSQLite.dll
gacutil -f -i PhpNetPDOSQLServer.dll
gacutil -f -i PhpNetSQLite.dll
gacutil -f -i PhpNetZip.dll
