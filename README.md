# UserManagementAPI — ASP.NET Core Web API with Custom Middleware

Built with Microsoft Copilot for Microsoft's *Back-End Development with .NET* course.

## Implemented Features
1. **CRUD Endpoints (`UsersController.cs`)**:
   - `GET /api/users` — List all users (with pagination/filter support)
   - `GET /api/users/{id}` — Retrieve user by ID (returns `404 NotFound` if missing)
   - `POST /api/users` — Create a new user with DataAnnotations model validation
   - `PUT /api/users/{id}` — Update existing user details
   - `DELETE /api/users/{id}` — Delete user record
2. **Input Validation (`User.cs`)**:
   - Validates `FullName`, `Email` (`[EmailAddress]`), `Role`, and `Department`, rejecting malformed payloads with structured HTTP `400 Bad Request` responses.
3. **Custom Middleware Pipeline (`Program.cs`, `CustomMiddlewares.cs`)**:
   - **ErrorHandlingMiddleware**: Global `try/catch` returning standardized JSON `500 Internal Server Error` payloads.
   - **TokenAuthenticationMiddleware**: Validates Bearer tokens on `/api/*` requests and returns `401 Unauthorized` when missing or invalid.
   - **RequestLoggingMiddleware**: Logs HTTP method, request path, status code, and execution elapsed milliseconds.
4. **Copilot Debugging & Optimization**:
   - Resolved null-reference hazards, added thread-safe `ConcurrentDictionary<int, User>` storage, and fixed middleware execution order (`ErrorHandling -> Authentication -> Logging -> MapControllers`).

## Running Locally
```bash
dotnet restore
dotnet run
```
