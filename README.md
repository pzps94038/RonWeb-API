# RonWeb-API
```
RonWeb 後端
```
## 資料庫相關-CodeFirst
- 更新資料庫
  1. 確認有無安裝EF Core CLI
  2. command 打 dotnet ef 有安裝跳到第4步驟
  3. 安裝dotnet tool install --global dotnet-ef
  4. 根據需求替換$變數，$連線字串、$模型位置、$Context名稱，執行以下command
  5. dotnet ef dbcontext scaffold "$連線字串" Pomelo.EntityFrameworkCore.MySql -o $模型位置 -f -c $Context名稱 --no-onconfiguring --no-pluralize