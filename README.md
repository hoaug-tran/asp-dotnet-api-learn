# TÀI LIỆU API .NET CORE

## Tài liệu tham khảo
- C# cơ bản: https://dotnettutorials.net/lesson/exception-handling-csharp/
- ASP.NET Core Web API: https://dotnettutorials.net/lesson/hypertext-transport-protocol/

Chú ý: Đọc chi tiết tài liệu API .NET Core tương ứng với từng phần bên dưới.

---

## PHẦN 1 — NỀN TẢNG CƠ BẢN (C# + .NET) (1 tuần)

- [x] 1. Học C# căn bản & lập trình OOP
  - [x] Kiểu dữ liệu, biến, điều kiện, vòng lặp
  - [x] Class / Property / Method
  - [x] OOP: Inheritance, Abstraction, Interface, Polymorphism
  - [x] async / await & Task ( hiểu cơ bản )
  - [x] LINQ cơ bản

- [x] 2. Cài đặt môi trường .NET
  - [x] .NET SDK (Microsoft)
  - [x] Visual Studio 2022 / VS Code
  - [x] SQL Server / LocalDB

---

## PHẦN 2 — BƯỚC ĐẦU VỚI ASP.NET CORE WEB API

- [x] 3. World Web API & Routing
  - [x] Tạo project Web API
  - [x] Routing và endpoint
  - [x] Model binding
  - [x] HTTP methods (GET / POST / PUT / DELETE)

- [x] 4. Minimal API

- [x] 5. Middleware & Pipeline
  - [x] Custom middleware
  - [x] Logging
  - [x] Exception handling

---

## PHẦN 3 — LÀM VIỆC VỚI CƠ SỞ DỮ LIỆU

- [x] 6. Entity Framework Core (ORM)
  - [x] DbContext, DbSet
  - [x] Migrations
  - [x] CRUD với LINQ
  - [x] Relationships (1-n, n-n)
  - [x] Query performance (AsNoTracking)

- [x] 7. Paging, Filtering & Sorting API
  - [x] Pagination
  - [x] Filter query params
  - [x] Sorting

---

## PHẦN 4 — KIẾN TRÚC & XÂY DỰNG ỨNG DỤNG SẠCH

- [ ] 8. Clean / Onion Architecture
  - [ ] API (Controllers)
  - [ ] Application (Services / DTO)
  - [ ] Domain (Entities / Business Logic)
  - [ ] Infrastructure (EF, Repository)

- [ ] 9. Repository & Unit of Work
  - [ ] Generic repository
  - [ ] Interface abstraction
  - [ ] Unit of Work

- [ ] 10. AutoMapper (Entity ↔ DTO)

Tài liệu tham khảo AutoMapper:
- https://dotnettutorials.net/course/asp-net-core-web-api-tutorials/

---

## PHẦN 5 — SECURITY & AUTHENTICATION

- [ ] 11. JWT Authentication
  - [ ] Login & JWT generation
  - [ ] Refresh token
  - [ ] Role / Policy

- [ ] 12. Authorization nâng cao
  - [ ] Policy
  - [ ] Claims
  - [ ] Permission-based authorization

---

## PHẦN 6 — CACHE, LOGGING & XỬ LÝ NÂNG CAO

- [ ] 13. Cache & Performance
  - [ ] In-Memory Cache
  - [ ] Distributed Cache (Redis)
  - [ ] Response Caching

- [ ] 14. Logging & Error Handling
  - [ ] Serilog / NLog
  - [ ] Global exception handler

---

## PHẦN 7 — TRIỂN KHAI & DEVOPS

- [ ] 15. Swagger / OpenAPI
  - [ ] Generate API documentation

- [ ] 16. Dockerize API
  - [ ] Dockerfile
  - [ ] Docker Compose

- [ ] 17. CI/CD Deployment
  - [ ] GitHub Actions
  - [ ] GitLab CI
