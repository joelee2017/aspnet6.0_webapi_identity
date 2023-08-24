# aspnet6.0_webapi_identity

asp.net 6.0 webapi 整合 jwt swagger identity

- 參考文件1：https://ithelp.ithome.com.tw/articles/10299318
- 參考文件2：https://www.c-sharpcorner.com/article/jwt-authentication-and-authorization-in-net-6-0-with-identity-framework/
- 參考文件3：https://jasonwatmore.com/post/2022/11/28/net-6-apply-authorize-attribute-to-all-controllers
- 附上 postman
- 總共兩種方式，結果一樣
  - Program.cs
    - AddJwtBearer
      - 對應1 WeatherForecastController > Login
      - 對應2 WeatherForecastController > Login2
  - 使用辦法
    - call WeatherForecast > Login or Login2
    - call WeatherForecast > username
      - 附加 Authorization Token = jwt token
    - 回傳結果
- 更新：
  - 提供客製化 
    - Authorize Attribute
    - Middleware
