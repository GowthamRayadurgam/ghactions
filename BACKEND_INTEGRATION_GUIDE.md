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

### Why No CORS?

When deployed to Azure Static Web Apps with a linked API backend, SWA handles routing and CORS automatically. The frontend communicates through SWA's `/api` route prefix, which proxies requests to the Function App backend. CORS headers are not needed with this native integration.

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
func azure functionapp publish <Functionapp name> --build remote --force
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

2. **Configure API endpoint:**
   - **Local Development:** Set `VITE_API_BASE_URL=http://localhost:7071/api` in `.env.local`
   - **Azure Deployment:** No environment variable needed! SWA automatically routes `/api` requests to the linked Function App backend.

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
   - Link Function App as API backend to SWA (no env var needed)
   - Test communication from the deployed SWA

## Environment Variables

### Frontend (.env.local)

| Variable | Purpose | Usage |
|----------|---------|-------|
| `VITE_API_BASE_URL` | Azure Function App API endpoint | **Local dev only** - Set to `http://localhost:7071/api`. For Azure deployment, SWA handles routing automatically via linked API backend config. |

### SWA Configuration (Azure Static Web Apps)

No environment variables needed for the Function App URL! Instead:
1. Link your Function App as an API backend in the SWA resource
2. SWA automatically creates `/api` proxy routes to your Function App
3. Your frontend calls `/api/data` and `/api/health` - SWA handles the routing
4. No CORS issues because requests stay within Azure infrastructure

## Triggers and Routes

| Function | Route | Methods | Auth Level |
|----------|-------|---------|-----------|
| GetData | `/api/data` | GET, POST | Anonymous |
| HealthCheck | `/api/health` | GET | Anonymous |

## Deployment to Azure

### Step 1: Link Function App to Static Web Apps

When using Azure Static Web Apps, you link the Function App as an API backend. SWA automatically handles routing and proxying:

```bash
# Using Azure CLI
az staticwebapp linkedbackend link \
  --name <swa-name> \
  --resource-group <resource-group> \
  --backend-resource-id "/subscriptions/<sub-id>/resourceGroups/<rg>/providers/Microsoft.Web/sites/<function-app-name>"
```

Or via Azure Portal:
1. Open your Static Web App resource
2. Go to "Configuration" → "Linked backends"
3. Add your Function App resource
4. SWA automatically proxies `/api/*` requests to your Function App

### Step 2: Update Frontend API Calls (Optional)

For deployed instances using SWA's API linking, update `apiService.js`:

```javascript
// Use relative path - SWA proxies it to your Function App
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || '/api';
```

This way:
- **Local dev**: Uses `http://localhost:7071/api` from `.env.local`
- **Azure deployed**: Uses `/api` which SWA automatically proxies to your Function App

No CORS needed because the request stays within Azure infrastructure!

## Next Steps

1. Add authentication (Azure AD/Entra ID) via SWA authentication
2. Implement request/response validation
3. Add error handling and retry logic
4. Set up API versioning
5. Add logging and monitoring with Application Insights
