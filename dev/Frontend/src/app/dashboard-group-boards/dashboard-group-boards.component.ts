import { Component, OnInit, NgModule } from '@angular/core';
import { NbAccordionModule, NbIconModule } from '@nebular/theme';
import { faMinusSquare, faPlusSquare, faTasks, faCogs, faClipboardCheck, faIdCardAlt, faUsers } from '@fortawesome/free-solid-svg-icons';
import { GroupsService } from '../api';
@Component({
  selector: 'app-dashboard-group-boards',
  templateUrl: './dashboard-group-boards.component.html',
  styleUrls: ['./dashboard-group-boards.component.scss']
})

@NgModule({
  imports: [
    NbAccordionModule,
    NbIconModule
  ],
})
export class DashboardGroupBoardsComponent implements OnInit {

  userGroups = [];
  groupsIcon = faUsers;
  loaded = false;

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
      this.LoadUserGroups();
  }

  private async LoadUserGroups() {
    this.userGroups = await this.groupsService.getCurrentUserGroups().toPromise();
    if (this.userGroups !== undefined && this.userGroups !== null) {
        this.loaded = true;
    }
  }
}
