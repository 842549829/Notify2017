/*
sql规范 
所有关键字大写 
表面跟字段跟数据库对应
一条sql语句结束必须跟;号
*/


/*关键字处理*/
--sqlserver
SELECT * FROM [Date];
--mysql 
SELECT * FROM `Date`;
-- oracle
SELECT * FROM "Date";

/*分页*/
--sqlserver
SELECT * FROM (SELECT *, ROW_NUMBER() OVER(Order by a.CreateTime DESC ) AS RowNumber FROM table_name AS a ) AS b WHERE RowNumber BETWEEN @PageIndex AND @PageSiez; 
--mysql 
SELECT * FROM T LIMIT @pageIndex, @PageSize;

varchar(max)
--sqlserver支持
--mysql不支持请用text 类型 

uniqueidentifier
--sqlserver支持
--mysql不支持请用varchar类型 

/*参数化*/
--sqlserver 用@符号

--mysql  用?符号or@符号


/*视图*/
--sqlserver 支持
--mysql  不支持

/*事件*/
--sqlserver 不支持
--mysql  支持