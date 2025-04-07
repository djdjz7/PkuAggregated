# PKU Aggregated

> C# 程序设计及其应用 (24-25学年第 1 学期) 大作业

一个典型前端 + 非典型后端的校内信息聚合平台。

## 搜素源

- 树洞
- 未名 BBS
- 课程测评条目
- 单位公告
- 学校公告
- ~~门户应用（开发前期可用，后期突然不能用了）~~


## 快速本地部署

> [!IMPORTANT]
> 
> 出于安全性考虑，本项目后端目前不支持多用户。

### 系统要求（Development）
- 前端：[Bun](https://bun.sh/)
- 后端：[.NET 8.0](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0)

### 前端

```sh
cd frontend
bun install
bun dev
```
应当看到如下输出：

```
  VITE v5.4.2  ready in 584 ms

  ➜  Local:   http://localhost:5173/
  ➜  Network: use --host to expose
  ➜  Vue DevTools: Open http://localhost:5173/__devtools__/ as a separate window
  ➜  Vue DevTools: Press Option(⌥)+Shift(⇧)+D in App to toggle the Vue DevTools
```

其中 `Local:   http://localhost:5173/` 指示了前端地址。

### 后端

#### 生成
```sh
cd PkuAggregated
dotnet build --configuration Release
cd bin/Release/net8.0/
```

#### 配置
**在启动之前，需要先在本地配置凭据，这包括：**
- 学号
- 门户密码
- BBS 用户名
- BBS 密码
- 验证令牌生成源数据

> [!NOTE]
>
> **关于验证令牌生成源数据：**
> - 验证令牌用于防止他人向您的服务器发出恶意请求导致您的账号被封禁。
> - 在前端和后端分别配置生成源数据后，前后端将自动生成验证令牌并校验。
> - 请不要泄露生成源数据，否则可能影响您的门户账号和 BBS 账号安全。
> - 只有在非 Debug 模式下，后端才会验证令牌。

在 `${PROJECT_ROOT}/PkuAggregated/bin/Release/net8.0/` 下，新建 `usersettings.json`，写入以下内容：

```json
{
  "FrontEndUrl": "http://localhost:5173 [若前端地址与此不一致，请使用您的前端地址]",
  "TokenGeneratorSource": "[验证令牌生成源数据]",
  "EnableCors": true,
  "AccountId": "[门户账号（学号）]",
  "Password": "[门户密码]",
  "BbsUsername": "[BBS 用户名]",
  "BbsPassword": "[BBS 密码]"
}
```

其中：`BbsUsername` 和 `BbsPassword` 是可选项，若不填入这两项，搜索结果将不包含未名 BBS。

#### 启动
```sh
./PkuAggregated
```

应当看到如下输出：

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
```

其中 `Now listening on: http://localhost:5000` 指示了后端地址。


### 访问

在浏览器中访问前端地址，遵循提示填入后端地址与验证令牌生成数据源，即可访问聚合平台。
