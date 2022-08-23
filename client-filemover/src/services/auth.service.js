import axios from "axios";

export const getTokenByCode = function getTokenByCodeFunc() {
  return axios.get(
    "http://localhost:8081/api/auth/login?clientId=s9QFPp8IRVtdrrHYa17LS0xdQ124v2wC&clientSecret=p7yCTmzT2eA0B3A5",
    {
      headers: {
        // remove headers
      },
    }
  );
};
