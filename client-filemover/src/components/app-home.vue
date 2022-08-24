<template>
  <v-main>
    <v-card width="800" class="mx-auto mt-5">
      <h1>Hubs</h1>
      <h3 v-if="chosenHub">Hubs id: {{ chosenHub }}</h3>
      <h3 v-if="chosenProject">Project id: {{ chosenProject }}</h3>
      <v-card-actions>
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
  }),
  methods: {
    async getFiles() {
      const hubsresponse = await forgeService.getHubsList();
      console.log(hubsresponse);
      this.hubs.push(hubsresponse);
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
      console.log(this.items);
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
          name: content.attributes.name,
          type: content.type,
          children: [],
        };
        this.items[0].children[0].children.push(contentJSON);
      }
    },
  },
};
</script>
