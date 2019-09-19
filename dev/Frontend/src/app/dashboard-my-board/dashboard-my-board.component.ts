import { Component, OnInit, NgModule } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem, CdkDrag } from '@angular/cdk/drag-drop';
import { NbAccordionModule, NbIconModule } from '@nebular/theme';
import { faMinusSquare, faPlusSquare, faTasks, faCogs, faClipboardCheck, faUserCog, faUsers } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-dashboard-my-board',
  templateUrl: './dashboard-my-board.component.html',
  styleUrls: ['./dashboard-my-board.component.scss']
})

@NgModule({
  imports: [
    NbAccordionModule,
    NbIconModule
  ],
})
export class DashboardMyBoardComponent implements OnInit {

  // Icons
  removeTaskIcon = faMinusSquare;
  addTaskIcon = faPlusSquare;
  openTasksHeaderIcon = faTasks;
  inProgressHeaderIcon = faCogs;
  doneHeaderIcon = faClipboardCheck;
  userTasksIcon = faUserCog;
  groupsIcon = faUsers;

  title = 'Simple Workflow management systems';
  all = ['Task 1', 'Task 2', 'Task 3'];
  even3 = ['Task 10', 'Task 11', 'Task 12', 'Task 13'];
  even = ['Task 19', 'Task 20', 'Task 21', 'Task 22', 'Task 23'];

  constructor() { }

  ngOnInit() {

  }

  public AddTask() {

  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {

      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
                        event.container.data,
                        event.previousIndex,
                        event.currentIndex);
    }
  }
    /** Predicate function that only allows even numbers to be dropped into a list. */
    evenPredicate(item: CdkDrag<number>) {
      return true;
       // return item.data % 2 === 0;
     }

       /** Predicate function that doesn't allow items to be dropped into a list. */
      noReturnPredicate() {
        return true;
  }

}
