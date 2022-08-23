import axios from "axios";

export const getHubs = function getHubsRequest() {
  return axios.get("http://localhost:8081/api/hubs", {
    headers: {
      // remove headers
    },
  });
};
