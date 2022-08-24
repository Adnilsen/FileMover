export default class ProjectAttributes {
  id = "null";
  name = "";

  constructor(id, name) {
    this.id = id;
    this.name = name;
  }

  getId() {
    return this.id;
  }
  getName() {
    return this.name;
  }
}
