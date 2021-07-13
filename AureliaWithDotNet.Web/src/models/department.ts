enum DepartmentEnum {
  HQ = 1,
  Store1 = 2,
  Store2 = 3,
  Store3 = 4,
  MaintenanceStation = 5
}

export class Department {

  get list() {
    var items = [];
    for (let item in DepartmentEnum) {
      if (isNaN(Number(item))) {
        items.push({ id: Number(DepartmentEnum[item]), name: item })
      }
    }
    return items;
  }

  get listNumbers() {
    var items = [];
    for (let item in DepartmentEnum) {
      if (!isNaN(Number(item))) {
        items.push(Number(item));
      }
    }
    return items;
  }
}
