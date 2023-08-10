<template>
  <v-main>
    <v-card width="800" class="mx-auto mt-5">
      <h1>Hubs</h1>
      <h2>User profile: {{userName}}</h2>
      <h3 v-if="chosenHub">Hubs id: {{ chosenHub }}</h3>
      <h3 v-if="chosenProject">Project id: {{ chosenProject }}</h3>
      <v-card-actions>
        <v-btn class="mr-4" @click="getUserProfile">Get Users</v-btn>

        <v-btn class="mr-4" @click="getFiles">Get files</v-btn>
      </v-card-actions>
      <v-row
        class="pa-4"
        justify="space-between"
      >
        <v-col cols="5">
          <v-treeview 
            v-if="items" 
            :active.sync="active"
            :open.sync="open"
            :items="items" 
            color="warning" 
            activatable
            open-on-click
            @update:active="selectedFileClicked"
            transition>
           
          </v-treeview>
        </v-col>
        <v-divider vertical></v-divider>

      <v-col
        class="d-flex text-center"
      >
        <v-scroll-y-transition mode="out-in">
          <div
            v-if="!selected"
            class="text-h6 grey--text text--lighten-1 font-weight-light"
            style="align-self: center;"
          >
            Select a File
          </div>
          <v-card
            v-else
            :key="selected.id"
            class="pt-6 mx-auto"
            flat
            max-width="400"
          >
            <v-card-text>
              
              <h3 class="text-h5 mb-2">
                Download File {{ selectedFile }}
              </h3>
              <v-btn
                class="ma-2"
                :loading="loading"
                :disabled="loading"
                color="secondary"
                @click="downloadFile(selectedFile)"
              >
                Download File
              </v-btn>
            
            </v-card-text>
            
          </v-card>
        </v-scroll-y-transition>
      </v-col>
    </v-row>
      
    </v-card>
  </v-main>
</template>

<script>
import * as forgeService from "@/services/forge.service";

export default {
  data: () => ({
    active: [],
    open: [],
    loading: false,

    hubs: [],
    chosenHub: "",
    projects: [],
    chosenProject: "",
    folders: "",
    chosenFolder: "",
    items: [],
    chosenVersion: [],
    selectedFile: "",
    userName: ""
  }),
  computed: {
    selected () {
        if (!this.active.length) return undefined;
        const id = this.active[0]
        return id != null;
      },
    },
  created() {
   this.getUserProfile()
  },
  methods: {
    selectedFileClicked () {
        this.selectedFile = this.active[0]
    },

    async getUserProfile() {
      const userresponse = await forgeService.getUserProfile();
      this.userName = userresponse.data.dictionary.emailId;
    },

    async getFiles() {
      const hubsresponse = await forgeService.getHubsList();
      this.hubs.push(hubsresponse.data[1]);
      console.log(this.hubs)
      this.chosenHub = this.hubs[0].id;
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

      this.chosenProject = this.projects[1].id;
      this.getProjectContent();
    },
    async getProjectContent() {
      const contentresponse = await forgeService.getProjectContentsList(
        this.chosenHub,
        this.chosenProject
      );
      /*for (let content of contentresponse.data) {
        const contentJSON = {
          id: content.id,
          name: content.attributes.name,
          type: content.type,
          children: [],
        };
        
      }*/
      // eslint-disable-next-line no-undef
      for (var id = 0; id < contentresponse.data.length; id++) {
        const contentJSON = {
          id: contentresponse.data[id].id,
          name: contentresponse.data[id].attributes.displayName,
          type: contentresponse.data[id].type,
          children: [],
      };
        this.items[1].children.push(contentJSON);
    }
      this.chosenFolder = contentresponse.data[1]
      this.getFolderContent();
    },
    async getFolderContent() {
      const contentresponse = await forgeService.getProjectContentsList(
        this.chosenHub,
        this.chosenProject,
        this.chosenFolder.id
      );

      console.log(contentresponse.data.length)
      for (var id = 0; id < contentresponse.data.length; id++) {
        const contentJSON = {
          id: contentresponse.data[id].id,
          name: contentresponse.data[id].attributes.displayName,
          type: contentresponse.data[id].type,
          children: [],
        };
        console.log(id)
        this.items[1].children[1].children.push(contentJSON);

        if(contentJSON.type == "folders") {
          const contentresponse2 = await forgeService.getProjectContentsList(
            this.chosenHub,
            this.chosenProject,
            contentJSON.id
          );
          for (var id2 = 0; id2 < contentresponse2.data.length; id2++) {
            const contentJSON2 = {
            id: contentresponse2.data[id2].id,
            name: contentresponse2.data[id2].attributes.displayName,
            type: contentresponse2.data[id2].type,
            children: [],
            };
            this.items[1].children[1].children[id].children.push(contentJSON2);

            if(contentJSON2.type == "folders") {
              const contentresponse3 = await forgeService.getProjectContentsList(
                this.chosenHub,
                this.chosenProject,
                contentJSON2.id
              );
              for (var id3 = 0; id3 < contentresponse3.data.length; id3++) {
                const contentJSON3 = {
                id: contentresponse3.data[id3].id,
                name: contentresponse3.data[id3].attributes.displayName,
                type: contentresponse3.data[id3].type,
                children: [],
                };
                this.items[1].children[1].children[id].childern[id2].children.push(contentJSON3);
              }
            }
          }
        }
      }
      this.chosenFolder = this.items[1].children[10];
      //this.getFileVerison();
    },

    downloadFile(itemId) {
      this.loading = true;

      this.getFileVerison(itemId)
    },

    async getFileVerison(item) {
      const contentresponse = await forgeService.getFileVersion(
        this.chosenHub,
        this.chosenProject,
        item
      );
      console.log(contentresponse.data[1])
      this.getFileLink(contentresponse.data[0].relationships.storage.data.id);
    },



    async getFileLink(storageId) {

      // Find the index of "object:"
      const objectIndex = storageId.indexOf("object:") + "object:".length;

      // Find the index of "/"
      const slashIndex = storageId.indexOf("/", objectIndex);

      // Extract the substring between "object:" and "/"
      const bucketKey = storageId.substring(objectIndex, slashIndex);

      // Extract the substring after "/"
      const itemName = storageId.substring(slashIndex + 1);

      const contentresponse = await forgeService.getFileLinks(
        this.chosenHub,  
        bucketKey,
        itemName
      );
      if(contentresponse.status == 200) {
        alert("successfully downloaded the file")
        this.loading = false;
      }
    },
  },
};
</script>
