import axios from "axios";

export const getHubsList = function getHubsListFunc() {
  return axios.get("http://localhost:8081/api/hubs", {
    headers: {
      // remove headers
    },
    withCredentials: true,
  });
};

export const getProjectsList = function getProjectsListFunc(hubId) {
  return axios.get(`http://localhost:8081/api/hubs/${hubId}/projects`, {
    headers: {
      // remove headers
    },
    withCredentials: true,
  });
};

export const getProjectContentsList = function getProjectContentsListFunc(
  hubId,
  projectId,
  folderId
) {
  console.log(folderId);
  if (folderId != null) {
    return axios.get(
      `http://localhost:8081/api/hubs/${hubId}/projects/${projectId}/contents?folder_id=${folderId}`,
      {
        headers: {
          // remove headers
        },
        withCredentials: true,
      }
    );
  } else {
    return axios.get(
      `http://localhost:8081/api/hubs/${hubId}/projects/${projectId}/contents`,
      {
        headers: {
          // remove headers
        },
        withCredentials: true,
      }
    );
  }
};
