# 📚 智慧圖書管理系統
> Windows Programming (II) 期末專題  
> 以 C# WinForms 打造的輕量化智慧圖書館管理系統
<img width="370" height="252" alt="螢幕擷取畫面 2026-06-20 230256" src="https://github.com/user-attachments/assets/b3b10f9f-02a1-46e5-bbf0-bf939338cb4e" />
<img width="500" height="300" alt="螢幕擷取畫面 2026-06-20 230314" src="https://github.com/user-attachments/assets/0027a517-87e6-4d4e-9b14-bdd4695a4f75" />

---

## ✨ 功能特色

### 🔐 帳號系統
- 登入 / 註冊 / 忘記密碼（重設密碼）
- 帳號資料儲存於 `users.csv`
- 登入後主畫面顯示歡迎訊息與借閱數量

### 📖 書籍展示
- ListView 大圖示模式，每本書顯示封面圖片與書名
- 封面依書號自動對應（`covers/B0001.jpg` 等）
- 已借出書籍書名以紅字標示

### 🔍 智慧篩選
| 功能 | 說明 |
|---|---|
| 分類篩選 | 下拉式 CheckedListBox，支援多選聯動 |
| 熱門排行 | LINQ 依評分降冪，顯示 TOP 5 |
| 新書推薦 | 自動篩選 30 天內上架書籍 |

### 📋 我的借閱
- 登入後左側自動列出目前借閱中的書籍
- 顯示書名與應還日期，逾期以紅字 ⚠ 標示
- 點選直接連動右側詳情面板

### 📤 借還書系統
- 借閱自動帶入登入帳號，應還日期 +14 天
- 還書自動計算逾期天數，罰金每日 5 元
- 按鈕權限依借閱人身份動態控制：
  - 在館中 → 可借閱
  - 已借出（自己）→ 可歸還
  - 已借出（他人）→ 兩者皆不可按

### ⭐ 書籍評分
- 任何書籍皆可評分（1～5 星）
- 評分即時更新並寫入 CSV

### 🔊 聲音簡介
- 每本書對應 `audio/B000X.mp3`
- 透過 Windows Media Player 播放，支援切換播放／停止

---

## 🖥️ 系統需求

- Windows 10 / 11
- .NET 6.0 以上
- Visual Studio 2022

---

## 🚀 執行方式

1. Clone 此 Repository
2. 以 Visual Studio 2022 開啟 `Windowsproject.sln`
3. 按下 `F5` 執行
4. 預設帳號：`admin` / 密碼：`1234`

---

## 📁 專案結構
Windowsproject/

├── Form1.cs              # 主視窗（書籍展示、篩選、借還書）

├── LoginForm.cs          # 登入／註冊／忘記密碼

├── Book.cs               # 書籍資料模型 + CSV 序列化

├── DataManager.cs        # CSV 讀寫、路徑管理

├── Resources/

│   ├── Books.csv         # 書籍資料庫

│   └── users.csv         # 帳號資料庫

├── covers/               # 書籍封面圖片（B0001.jpg ...）

└── audio/                # 書籍聲音簡介（B0001.mp3 ...）

---

## 💾 資料格式
### users.csv
<img width="141" height="67" alt="image" src="https://github.com/user-attachments/assets/2c1cf3d8-8582-43d1-bfce-5c11b986d182" />

### Books.csv
<img width="1530" height="69" alt="image" src="https://github.com/user-attachments/assets/50160c48-8494-49bd-bde3-1e21e5fb12ee" />





---

## 📌 參考資源

- [Microsoft WinForms 官方文件](https://docs.microsoft.com/dotnet/desktop/winforms)
- [LINQ 教學](https://docs.microsoft.com/dotnet/csharp/programming-guide/concepts/linq)
- [Windows Media Player 控制項](https://docs.microsoft.com/windows/win32/wmp/windows-media-player-object-model-reference)
- [StreamReader / StreamWriter](https://docs.microsoft.com/dotnet/api/system.io.streamreader)
