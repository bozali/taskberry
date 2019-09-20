import { Component, OnInit, NgModule } from '@angular/core';
import { CdkDragDrop, moveItemInArray, transferArrayItem, CdkDrag } from '@angular/cdk/drag-drop';
import { NbAccordionModule, NbIconModule, NbToastrService } from '@nebular/theme';
import { faMinusSquare, faPlusSquare, faTasks, faCogs, faClipboardCheck, faIdCardAlt, faUsers } from '@fortawesome/free-solid-svg-icons';
import { TasksService } from '../api';
import { Task } from '../api/model/task';

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
  userTasksIcon = faIdCardAlt;
  groupsIcon = faUsers;

  title = 'Simple Workflow management systems';
  openTasksRow: Task[] = [];
  inProgressTasksRow: Task[] = [];
  doneTasksRow: Task[] = [];

  constructor(private tasksService: TasksService, private toastrService: NbToastrService) { }

  ngOnInit() {
    this.LoadAllTasks();
  }

  public async LoadAllTasks() {

    // Get Tasks from Database
     const userTasks = await this.tasksService.getCurrentUserTasks().toPromise();

     // Add to Table
     userTasks.forEach(task => {
      switch (task.status) {
        case 0:
          this.openTasksRow.push(task);
          break;
        case 1:
          this.inProgressTasksRow.push(task);
          break;
        case 2:
          this.doneTasksRow.push(task);
          break;
      }
     });
  }

  public AddTask(row: number) {

  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      // Add order change to DB
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);

    } else {
      this.dropTaskToNewRow(event);
    }
  }

  public async dropTaskToNewRow(event: CdkDragDrop<string[]>) {
    let taskId = event.item.data.id;
    let newStatus = 0;
    // Add Status/Row change to database
    switch (event.container.id) {
      case 'myOpenTasks':
          newStatus = 0;
          break;
      case 'myInProgressTasks':
          newStatus = 1;
          break;
      case 'myDoneTasks':
          newStatus = 2;
          break;
    }

    this.tasksService.moveTask(taskId, 0).toPromise();
    // Apply changes to board
    transferArrayItem(event.previousContainer.data,
                      event.container.data,
                      event.previousIndex,
                      event.currentIndex);
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

  public async RemoveTaskFromMyBoard(id: string, row: number) {

    // Remove from Database
    await this.tasksService.deleteTask(id).subscribe(result => {

    // Remove from Board
    switch (row) {
      case 0:
          const taskOpenIndex = this.openTasksRow.findIndex(w => w.id === id);
          if (taskOpenIndex !== -1) {
            this.openTasksRow.splice(taskOpenIndex, 1);
          }
          break;
      case 1:
          const taskInProgressIndex = this.inProgressTasksRow.findIndex(w => w.id === id);
          if (taskInProgressIndex !== -1) {
          this.inProgressTasksRow.splice(taskInProgressIndex, 1);
          }
          break;
      case 2:
          const doneTasksIndex = this.doneTasksRow.findIndex(w => w.id === id);
          if (doneTasksIndex !== -1) {
            this.doneTasksRow.splice(doneTasksIndex, 1);
          }
          break;
    }
    this.toastrService.primary(result.result, 'Aufgabe entfernt');
    });

  }

  public async AddTaskToMyBoard(row) {
    const userId = localStorage.getItem('userId');

    if (userId !== undefined && userId != null) {
      let newTask: Task = {
        title: 'Titel',
         description: 'Beschreibung',
         status: row,
         type: 0,
         ownerId: userId
        };

      // Add to Database
      newTask = await this.tasksService.createTask(newTask).toPromise();

      // Add to Board
      if (newTask !== undefined || newTask != null) {
        switch (row) {
          case 0:
            this.openTasksRow.push(newTask);
            break;
          case 1:
              this.inProgressTasksRow.push(newTask);
              break;
          case 2:
              this.doneTasksRow.push(newTask);
              break;
        }
      }
      this.toastrService.primary('Neue Aufgabe wurde erstellt!', 'Aufgabe erstellt');
    }
  }

}
