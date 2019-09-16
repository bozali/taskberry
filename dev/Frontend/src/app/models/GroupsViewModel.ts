export interface GroupsViewModelReplaceMeLater {
  GroupViewModel: GroupViewModel[];
}



export interface GroupViewModelReplaceMeLater {
  id?: number;
  name?: string;
  description?: string;
}

export class GroupViewModel {
  id?: number;
  name?: string;
  description?: string;

  constructor() {

}
}

export class GroupsViewModel {
  GroupViewModel: GroupViewModel[];

  constructor() {
    this.GroupViewModel = new Array<GroupViewModel>();
}
}
