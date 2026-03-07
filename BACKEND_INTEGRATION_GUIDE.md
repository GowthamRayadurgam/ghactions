# React Frontend + Azure Function App Backend Integration

This setup demonstrates communication between a React Static Web App frontend and an Azure Function App backend.

## Architecture

- **Frontend**: React app deployed to Azure Static Web Apps (react-proj2)
- **Backend**: .NET Azure Function App with CORS-enabled HTTP triggers
- **Communication**: REST API calls from React to Function App

## Backend Setup (Azure Function App)

### Functions Available

1. **GetData** (`/api/data`)
   - HTTP GET/POST
   - Returns JSON with message, timestamp, version, and status
   - Example response:
     ```json
     {
       "message": "Hello from Azure Function App!",
       "timestamp": "2026-03-07T10:30:00Z",
       "version": "1.0",
       "status": "success"
     }
     ```

2. **HealthCheck** (`/api/health`)
   - HTTP GET
   - Returns service health status
   - Example response:
     ```json
     {
       "status": "healthy",
       "timestamp": "2026-03-07T10:30:00Z"
     }
     ```

### CORS Configuration

Both functions include CORS headers to allow requests from the frontend:
- `Access-Control-Allow-Origin: *`
- `Access-Control-Allow-Methods: GET, POST, OPTIONS`
- `Access-Control-Allow-Headers: Content-Type`

### Local Development

```bash
cd backend/functionapp
func start
# Function App runs on http://localhost:7071
```

### Deploy to Azure

```bash
# Create Function App
az functionapp create --resource-group <rg-name> --consumption-plan-name <plan-name> --runtime dotnet --runtime-version 8 --functions-version 4 --name <function-app-name>

# Deploy code
func azure functionapp publish <function-app-name>
```

## Frontend Setup (React)

### API Service

The `src/services/apiService.js` provides methods:
- `getData()` - Calls `/api/data` endpoint
- `checkHealth()` - Calls `/api/health` endpoint

### BackendTest Component

A dedicated component (`src/components/BackendTest.jsx`) for testing backend communication:
- Buttons to trigger API calls
- Displays response JSON
- Shows error messages
- Loading states

### Usage

1. Add the component to your app:
   ```jsx
   import BackendTest from './components/BackendTest';
   
   // In your main content or app
   <BackendTest />
   ```

2. Configure API endpoint in `.env.local`:
   ```
   # Local development
   VITE_API_BASE_URL=http://localhost:7071/api
   
   # Production (Azure)
   VITE_API_BASE_URL=https://your-function-app-name.azurewebsites.net/api
   ```

## Testing Flow

1. **Local Development**
   ```bash
   # Terminal 1: Start Function App
   cd backend/functionapp
   func start
   
   # Terminal 2: Start React dev server
   cd react-proj2
   npm run dev
   ```
   
2. **In Browser**
   - Navigate to `http://localhost:5173`
   - Find the "Backend Communication Test" section
   - Click "Get Data" or "Check Health" buttons
   - View the JSON responses

3. **SWA + Function App (Deployed)**
   - Deploy React app to Azure Static Web Apps
   - Deploy Function App to Azure
   - Update `.env.local` with Function App URL
   - Test communication from the deployed SWA

## Environment Variables

### Frontend (.env.local)

| Variable | Purpose | Default |
|----------|---------|---------|
| `VITE_API_BASE_URL` | Azure Function App API endpoint | `http://localhost:7071/api` |

## Triggers and Routes

| Function | Route | Methods | Auth Level |
|----------|-------|---------|-----------|
| GetData | `/api/data` | GET, POST | Anonymous |
| HealthCheck | `/api/health` | GET | Anonymous |

## Next Steps

1. Add authentication (Azure AD/Entra ID)
2. Implement request/response validation
3. Add error handling and retry logic
4. Set up API versioning
5. Add logging and monitoring with Application Insights
