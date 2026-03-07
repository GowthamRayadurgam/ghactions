import { useState } from 'react';
import { apiService } from '../services/apiService';
import '../styles/BackendTest.css';

function BackendTest() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  async function fetchData() {
    setLoading(true);
    setError(null);
    try {
      const result = await apiService.getData();
      setData(result);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  async function checkHealth() {
    setLoading(true);
    setError(null);
    try {
      const result = await apiService.checkHealth();
      setData(result);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="backend-test">
      <h2>Backend Communication Test</h2>
      <div className="button-group">
        <button onClick={fetchData} disabled={loading}>
          {loading ? 'Loading...' : 'Get Data'}
        </button>
        <button onClick={checkHealth} disabled={loading}>
          {loading ? 'Checking...' : 'Check Health'}
        </button>
      </div>

      {error && <div className="error">{error}</div>}

      {data && (
        <div className="response">
          <h3>Response:</h3>
          <pre>{JSON.stringify(data, null, 2)}</pre>
        </div>
      )}
    </div>
  );
}

export default BackendTest;
