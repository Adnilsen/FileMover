<template>
  <v-main>
    <v-card width="800" class="mx-auto mt-5">
      <h1>Hubs</h1>
      <h3 v-if="chosenHub">Hubs id: {{ chosenHub }}</h3>
      <h3 v-if="chosenProject">Project id: {{ chosenProject }}</h3>
      <v-card-actions>
        <v-btn class="mr-4" @click="getUserProfile">Get Users</v-btn>

        <v-btn class="mr-4" @click="getFiles">Get files</v-btn>
      </v-card-actions>
      <v-treeview v-if="items" color="warning" :items="items"></v-treeview>
    </v-card>
  </v-main>
</template>

<script>
import * as forgeService from "@/services/forge.service";

export default {
  data: () => ({
    hubs: [],
    chosenHub: "",
    projects: [],
    chosenProject: "",
    folders: "",
    chosenFolder: "",
    items: [],
    chosenVersion: [],
  }),
  methods: {

    async getUserProfile() {
      const userresponse = await forgeService.getUserProfile();
      console.log(userresponse.data);
    },

    async getFiles() {
      const hubsresponse = await forgeService.getHubsList();
      console.log(hubsresponse);
      this.hubs.push(hubsresponse);
      console.log(this.hubs[0].data[0]);

      this.chosenHub = this.hubs[0].data[0].id;
      for (let hub of this.hubs[0].data) {
        console.log(hub.id);
      }
      console.log(this.hubs[0].data[0].id);
      this.getProjects();
    },
    async getProjects() {
      const projectsresponse = await forgeService.getProjectsList(
        this.chosenHub
      );
      for (let projectsResponse of projectsresponse.data) {
        this.projects.push({
          id: projectsResponse.id,
          name: projectsResponse.attributes.name,
        });
        this.items.push({
          id: projectsResponse.id,
          name: projectsResponse.attributes.name,
          children: [],
        });
      }
      console.log(this.projects);

      this.chosenProject = this.projects[0].id;
      this.getProjectContent();
    },
    async getProjectContent() {
      const contentresponse = await forgeService.getProjectContentsList(
        this.chosenHub,
        this.chosenProject
      );
      for (let content of contentresponse.data) {
        const contentJSON = {
          id: content.id,
          name: content.attributes.name,
          type: content.type,
          children: [],
        };
        this.items[0].children.push(contentJSON);
      }
      this.chosenFolder = this.items[0].children[0];
      this.getFolderContent();
    },
    async getFolderContent() {
      const contentresponse = await forgeService.getProjectContentsList(
        this.chosenHub,
        this.chosenProject,
        this.chosenFolder.id
      );

      for (let content of contentresponse.data) {
        const contentJSON = {
          id: content.id,
          name: content.attributes.displayName,
          type: content.type,
          children: [],
        };
        this.items[0].children[0].children.push(contentJSON);
      }
      this.chosenFolder = this.items[0].children[0].children[10];
      this.getFileVerison();
    },

    async getFileVerison() {
      console.log(this.items[0].children[0].children[0].id + " her")
      const contentresponse = await forgeService.getFileVersion(
        this.chosenHub,
        this.chosenProject,
        this.items[0].children[0].children[0].id
      );
      this.getFileLink(contentresponse.data[0].relationships.storage.data.id);
    },

    async getFileLink(bucketId) {
      let bucketIdNew = encodeURIComponent(bucketId)
      console.log(bucketIdNew)
      
      const contentresponse = await forgeService.getFileLinks(
        this.chosenHub,  
        bucketIdNew,
        "House Design.rvt"
      );
      console.log(contentresponse);
    },
  },
};
</script>
