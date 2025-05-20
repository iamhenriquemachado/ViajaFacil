export default {
  async checkAuthStatus() {
    try {
      const response = await fetch('https://localhost:7073/api/users/authentication/me', {
        method: 'GET',
        credentials: 'include', 
        headers: {
          'Content-Type': 'application/json'
        }
      });
      
      if (response.ok) {
        const userData = await response.json();
        return {
          isAuthenticated: true,
          user: userData
        };
      } else {
        return {
          isAuthenticated: false,
          user: null
        };
      }
    } catch (error) {
      console.error('Error checking authentication status:', error);
      return {
        isAuthenticated: false,
        user: null
      };
    }
  },
  
  async login(email, password) {
    const response = await fetch('https://localhost:7073/api/users/authentication', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      credentials: 'include',
      body: JSON.stringify({ email, password })
    });
    
    if (response.ok) {
      const data = await response.json();
      return {
        success: true,
        isAdmin: data.isAdmin
      };
    } else {
      return {
        success: false,
        error: response.status === 401 ? 'Invalid email or password' : 'Login failed'
      };
    }
  },
  
  async logout() {
    return { success: true };
  }
}
