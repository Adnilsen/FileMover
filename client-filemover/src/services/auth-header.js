export function authHeader() {
    // return authorization header with basic auth credentials
    const token = document.cookie = "public_token";
    if (token) {
      return { Authorization: `Bearer ${token}` }
    } else {
      return {}
    }
  }
  