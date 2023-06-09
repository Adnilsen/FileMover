import axios from "axios";

export const getTokenByCode = function getTokenByCodeFunc() {
  return axios.get(
    "http://localhost:8081/api/auth/login?clientId=DEIMuylAig37ffyHoM4q3nFuS6sA8y5S&clientSecret=rFYm8gzjyS98VkbS",
    {
      headers: {
        // remove headers
      },
    }
  );
};
