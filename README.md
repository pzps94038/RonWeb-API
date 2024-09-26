# RonWeb-API
```
RonWeb 後端
```
## 資料庫相關-CodeFirst
- 更新資料庫
  1. 確認有無安裝EF Core CLI
  2. command 打 dotnet ef 有安裝跳到第4步驟
  3. 安裝dotnet tool install --global dotnet-ef
  4. dotnet ef migrations add "異動說明"
  5. 根據需求替換$變數，$連線字串、$Port、$資料庫名稱、$帳號、$密碼
  5. dotnet ef database update --connection "Server=$連線字串;Port=$Port;Database=$資料庫名稱;Uid=$帳號;Pwd=$密碼;"