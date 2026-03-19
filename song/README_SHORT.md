# 🎵 Music Management API

אפליקציית ניהול שירים עם סנכרון בזמן אמת בין כרטיסיות ומכשירים.

## 📋 תיאור כללי

אפליקציה לניהול שירים (CRUD) הבנויה עם **ASP.NET Core 9.0** ו-**HTML/JavaScript** בחזית.
- משתמשים יכולים להוסיף, לערוך ולמחוק שירים
- כל השינויים מסתנכרנים בזמן אמת בכל הכרטיסיות של המשתמש באמצעות **SignalR**
- מערכת אימות עם **JWT** ותפקידים (admin/user)

## 🏗️ מבנה הפרויקט

```
Controllers/      → API controllers (Songs, Users, Login)
Models/          → songType, userType
Services/        → Business logic + SignalR notifications
Hubs/            → ActivityHub (SignalR)
Data/            → Songs.json, Users.json (Database)
wwwroot/         → HTML/CSS/JavaScript frontend
```

## 🛠️ Stack טכנולוגי

- **Backend**: ASP.NET Core 9.0, JWT Authentication
- **Frontend**: HTML5, CSS3, Vanilla JavaScript
- **Real-time**: SignalR
- **Database**: JSON files

## 🚀 שימוש

1. **הרצה**: `dotnet run`
2. **דפדפן**: `https://localhost:7xxx`
3. התחבר או צור חשבון
4. הוסף/ערוך/מחק שירים

## 🔄 כיצד עובד SignalR

כשמשתמש מוסיף שיר בטאב אחד:
1. השרת מקבל את הפעולה
2. שולח update לכל הטאבים של אותו משתמש דרך SignalR
3. כל הטאבים מרעננים את הרשימה בעצמאות

## 🔐 אימות

- כל ה-API דורש JWT token
- משתמשים רגילים רואים רק שירים שלהם
- Admin רואה את כל השירים

**Last Updated**: March 2026
