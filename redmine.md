# Oauth2.0 认证的Web api
非常简单的OAuth2认证Demo，首次启动的时候会加载管理NuGet，保持网络链接

### VS创建过程
1. 新建Web项目
2. 选择模板Empty ，选择Web API
3. 管理NuGet  
  Microsoft.Owin  
  Microsoft.Owin.Cors  
  Microsoft.Owin.Security.OAuth  
  Microsoft.AspNet.WebApi.Owin  
  Microsoft.Owin.Host.SystemWeb  
  Microsoft.AspNet.Identity.Owin  
4. 新建Startup，删除Global.asax
5. 测试，可以用Postman
  1. 获取token
  ```
  url: http://localhost:57387/token,
  type: POST,
  body:{
  "grant_type": "password",
  "username": "admin",
  "password": "123456"
  }
  ```
  2. 测试结果
  ```
  {
    "access_token": "tfIILTn5PaK7iQnINWI6opW4que_oo08YkTww5v3nfge6oHtbS1BfRqmF1VGj_-40Uj8i6cPq8QcPqMX1CBYmrSvhrjbx4HKwp3n2J58WiIU5RtqylQ5xG6xJ35cty0moeML_eAj5ZRz-F1MGCIZS6DGnOu6CpH4w4h46l9h6DSD5tjq1h1UpD8mYLI1fcHBDmW_bpgBL18DPioVp8KhEQKVnn_ZaLwKVuHxxDJRUS4nJ0et5GFiIeiecmRUNEaAEWyYKRFvAwSF1IRlxkbYkCGLKo3hNLWwYZeyR6BwrbD1UsfVchRnAS4LXY_FEaUW",
    "token_type": "bearer",
    "expires_in": 99,
    "refresh_token": "1b4d5bff98494ae4a38b9bffb99f7b73"
  }
  ```
  3. 刷新token
  ```
  url: http://localhost:57387/token,
  type: POST,
  body:{
  "grant_type": "refresh_token",
  "refresh_token": "1b4d5bff98494ae4a38b9bffb99f7b73", //得到的refresh_token，只能用一次
  }
  ```
  4. 测试API
  ```
  url: http://localhost:57387/api/default/GetTest,
  Header:{"Authorization":"bearer access_token"},//access_token就是获取的，与bearer之间有个空格
  ```

### 错误
1. 未能加载文件或程序集  
  解决办法：先删除通过packages.config相应的程序集，之后NuGet引用相应的程序集