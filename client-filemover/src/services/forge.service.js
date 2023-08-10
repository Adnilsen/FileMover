import axios from "axios";

export const getUserProfile = function getUserProfileFunc() {
  return axios.get("http://localhost:8081/api/auth/user", {
    headers: {
      // remove headers
    },
    withCredentials: true,
  });
};


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


export const getFileVersion = function getFileVersionsFunc(
  hubId,
  projectId,
  item
) {
  if (item != null) {
    return axios.get(
      `http://localhost:8081/api/hubs/${hubId}/projects/${projectId}/contents/${item}/versions`,
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

export const getFileLinks = function getFileLinksFunc(
  hubId,
  bucketKey,
  objectName
) {
  if (bucketKey != null) {
    return axios.get(
      `http://localhost:8081/api/hubs/${hubId}/bucket/${bucketKey}/contents/${objectName}/links`,
      {
        headers: {
          
        },
        responseType: 'stream',
        withCredentials: true,
      }
    );
  }
};

