# CIMInfoSer

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools

make sure project build success before run below command :

Scaffold-DbContext "Server=103.58.148.161,1433;initial catalog=cim_db;persist security info=True;user id=cim;password=4dev@cim;MultipleActiveResultSets=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force

Scaffold-DbContext "Server=.,4433;initial catalog=3m_dashboard_db;persist security info=True;user id=cim;password=4dev@cim;MultipleActiveResultSets=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Dashboard -force

# client command 
transfer-message

command-channel
{ "name": "reload-page"}
{ "param": "Hallooooo!!!!", "name": "alert"}
{ "param": "./maintainace", "name": "navigate"}
{ "param": "./production-plan-overview/PRODPLAN-1", "name": "navigate"}

#Redis
install https://github.com/microsoftarchive/redis/releases
Sample cmd on cli
scan 0
redis-cli -h 103.70.6.198 -p 6379

#Dapper
https://dapper-tutorial.net/


# run db using docker
docker-compose up -d
docker-compose ps
docker cp 000_create_db.sql dashboard-db:.
docker-compose exec dashboard-db bash
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Super_p4ssw0rd' -i 000_create_db.sql
