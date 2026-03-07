// API service for communicating with Azure Function App backend
// Local dev: uses VITE_API_BASE_URL env var pointing to local Function App
// Azure (SWA deployment): uses /api which SWA proxies to linked Function App backend
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || '/api';

export const apiService = {
  // Fetch data from the backend
  async getData() {
    try {
      const response = await fetch(`${API_BASE_URL}/data`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        throw new Error(`API error: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      console.error('Failed to fetch data:', error);
      throw error;
    }
  },

  // Check backend health
  async checkHealth() {
    try {
      const response = await fetch(`${API_BASE_URL}/health`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        throw new Error(`Health check failed: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      console.error('Health check failed:', error);
      throw error;
    }
  },
};
