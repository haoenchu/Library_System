# 智慧圖書管理系統

> Windows Programming (II) 期末專題

![C#](https://img.shields.io/badge/C%23-WinForms-blue)
![.NET](https://img.shields.io/badge/.NET-6.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 專案簡介

以 **C# Windows Forms** 打造的輕量化智慧圖書館管理系統。  
支援帳號登入、書籍封面瀏覽、借還書流程、逾期罰金計算、聲音簡介播放，  
所有資料透過 `CSV` 檔案持久化儲存，無需資料庫。

---

## 功能特色

| 模組 | 功能說明 |
|---|---|
| 帳號系統 | 登入 / 註冊 / 忘記密碼（重設），資料存於 `users.csv` |
| 書籍展示 | ListView 大圖示模式，封面圖片依書號自動對應 |
| 分類篩選 | CheckedListBox 下拉多選，支援隨時新增分類 |
| 熱門排行 | 依累積平均評分降冪排序，即時顯示 TOP 5 |
| 新書推薦 | 自動篩選 30 天內上架書籍 |
| 借閱書籍 | 借閱人綁定登入帳號，自動設定 +14 天應還日期 |
| 歸還書籍 | 自動計算逾期天數，逾期收費 5 元 / 天 |
| 書籍評分 | 多人累積平均評分，星星旁顯示小數平均值 |
| 聲音簡介 | WindowsMediaPlayer 播放 `.mp3`，可切換播放 / 停止 |
| 我的借閱 | 左側清單即時顯示目前登入帳號的借閱中書籍 |

---

## 🖥️ 系統需求

- Windows 10 / 11
- [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)
- Visual Studio 2022

---

## 🚀 執行方式

```bash
# 1. Clone 專案
git clone https://github.com/your-username/LibrarySystem.git

# 2. 以 Visual Studio 2022 開啟
LibrarySystem.sln

# 3. 按下 F5 執行
```

預設帳號：
```
帳號：123  
密碼：123
```

---

## 專案結構

```
Windowsproject/
├── Form1.cs                # 主視窗（書籍展示、篩選、借還書、評分）
├── Form1.Designer.cs       # Designer 自動產生
├── LoginForm.cs            # 登入 / 註冊 / 忘記密碼
├── Book.cs                 # 書籍資料模型 + CSV 序列化
├── DataManager.cs          # CSV 讀寫、路徑管理
├── Program.cs              # 程式進入點
│
├── Resources/
│   ├── Books.csv           # 書籍資料庫
│   └── users.csv           # 帳號資料庫
│
├── covers/                 # 書籍封面圖片（依書號命名，如 B0001.jpg）
└── audio/                  # 聲音簡介（依書號命名，如 B0001.mp3）
```

---

## CSV 格式

**Books.csv**
```
BookId,Title,Author,Category,Status,Borrower,DueDate,Rating,AddedDate,Description,RatingCount,RatingSum
B0001,解憂柑仔店,藤野圭吾,文學小說,在館中,,,,2026-06-01,"一間神奇的雜貨店...",0,0
```

| 欄位 | 說明 |
|---|---|
| BookId | 書號（自動流水號 B0001～） |
| Title | 書名 |
| Author | 作者 |
| Category | 分類（文學小說／資訊科技／商業理財／藝術設計） |
| Status | 在館中 / 已借出 |
| Borrower | 借閱人帳號 |
| DueDate | 應還日期（yyyy-MM-dd） |
| Rating | 四捨五入後的整數星級（1～5） |
| AddedDate | 上架日期 |
| Description | 書籍簡介 |
| RatingCount | 累積評分人數 |
| RatingSum | 累積評分總分 |

**users.csv**
```
Username,Password
admin,1234
```

---

## 借還書流程

```
選擇書籍 → 點「辦理借閱」→ 確認視窗（顯示應還日期）
        → 狀態變「已借出」→ 左側我的借閱清單更新

選擇書籍 → 點「歸還書籍」→ 計算逾期天數
        → 逾期：顯示罰金（5元/天）→ 確認後狀態變「在館中」
```

---

## 評分機制

- 任何人皆可對書籍評分（1～5 星）
- 採**累積平均**：`平均分 = RatingSum / RatingCount`
- 星星旁顯示小數平均值，例如 `★★★☆☆  3.7`
- 熱門排行依真正平均分排序

---

## 截圖

### 登入畫面
<img width="370" height="252" alt="螢幕擷取畫面 2026-06-20 230256" src="https://github.com/user-attachments/assets/f81ba3f8-e5be-4a3b-9d77-24ffa90c6f6a" />


### 主畫面
<img width="1330" height="757" alt="image" src="https://github.com/user-attachments/assets/bf28c50e-e09f-4447-8d91-77c90dc7bca8" />


---

## 資料來源

- Microsoft WinForms 文件：https://docs.microsoft.com/dotnet/desktop/winforms
- LINQ 教學：https://docs.microsoft.com/dotnet/csharp/programming-guide/concepts/linq
- CSV 讀寫參考：https://docs.microsoft.com/dotnet/api/system.io.streamreader
- Windows Media Player SDK：https://docs.microsoft.com/windows/win32/wmp/windows-media-player-sdk
