# Module Tool

目的用於增加資料庫需要的相關 `attribute` 到 class 中，並使用 `snake_case` 方式命名；

例如將以下類別：

```csharp
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
}
```

變成

```csharp
[Table("user")]
public class User
{
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("user_name")]
    public string UserName { get; set; }
    
    [Column("password")]
    public string Password { get; set; }
}
```

