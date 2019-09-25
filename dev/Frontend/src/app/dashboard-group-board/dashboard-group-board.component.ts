import { Component, OnInit, Input } from '@angular/core';
import { Group, TasksService, Task, UsersService } from '../api';
import { faMinusSquare, faPlusSquare, faTasks, faCogs, faClipboardCheck, faIdCardAlt, faUsers } from '@fortawesome/free-solid-svg-icons';
import { NbToastrService, NbDialogService } from '@nebular/theme';
import { CdkDragDrop, moveItemInArray, transferArrayItem, CdkDrag } from '@angular/cdk/drag-drop';
import { DashboardGroupChangeAssigneeComponent } from '../dashboard-group-change-assignee/dashboard-group-change-assignee.component';

@Component({
  selector: 'app-dashboard-group-board',
  templateUrl: './dashboard-group-board.component.html',
  styleUrls: ['./dashboard-group-board.component.scss']
})

export class DashboardGroupBoardComponent implements OnInit {

  public nameAndDescription = '';

  @Input() public group: Group;
  public tasks: Task[] = [];
  public loaded = false;
  removeTaskIcon = faMinusSquare;
  addTaskIcon = faPlusSquare;
  openTasksHeaderIcon = faTasks;
  inProgressHeaderIcon = faCogs;
  doneHeaderIcon = faClipboardCheck;
  userTasksIcon = faIdCardAlt;
  groupsIcon = faUsers;

  openTasksRow = [];
  inProgressTasksRow = [];
  doneTasksRow = [];

  currentUsers = [];

  constructor(private dialogService: NbDialogService,
              private userService: UsersService, private tasksService: TasksService, private toastrService: NbToastrService) { }

  ngOnInit() {
    this.LoadAllTasks();
    this.LoadAllUser();
  }

  private async LoadAllUser() {
    this.currentUsers = await this.userService.getUsersByGroupId(this.group.id).toPromise();
  }

  public GetUserName(userId) {
    const user = this.currentUsers.find(w => w.id === userId as number);
    if(user == undefined || user == null) {
      return 'Niemand';
    }
    else {
      return user.firstName + ' ' + user.lastName[0] + '.';
    }
  }
  private async LoadAllTasks() {

    // Get Tasks from Database
     this.tasks = await this.tasksService.getTasksFromGroup(this.group.id).toPromise();

     // Add to Table
     this.tasks.forEach(task => {
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
     this.loaded = true;
  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      // Add order change to DB
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);

    } else {
      this.dropTaskToNewRow(event);
    }
  }

  public ShowChangeAssigneeWindow(groupId, taskId) {
    const dialogRef = this.dialogService.open(DashboardGroupChangeAssigneeComponent,
      { hasBackdrop: true, closeOnBackdropClick: true});
    dialogRef.onClose.subscribe(result => {
        if  (result) {
          const taskIndex = this.tasks.findIndex(w=> w.id == taskId);
          this.tasks[taskIndex].assigneeId = result.assigneeId;
          if(taskIndex != -1) {
            switch(this.tasks[taskIndex].status){
              case 0:
              const openTaskIndex = this.openTasksRow.findIndex(w=> w.id == taskId);
              if(openTaskIndex != -1) {
                this.openTasksRow[openTaskIndex].assigneeId = result.assigneeId as number;
              }
              break;
              case 1:
              const inProgressTaskIndex = this.inProgressTasksRow.findIndex(w => w.id == taskId);
              if(inProgressTaskIndex != -1) {
                this.inProgressTasksRow[inProgressTaskIndex].assigneeId = result.assigneeId;
              }
              break;
              case 2:
              const doneTaskIndex = this.doneTasksRow.findIndex(w => w.id == taskId);
              if(doneTaskIndex != -1) {
                this.doneTasksRow[doneTaskIndex].assigneeId = result.assigneeId;
              }
              break;
            }
          }
        }
      });
    const editComp = dialogRef.componentRef.instance;
    editComp.allUser = this.currentUsers;
    editComp.loaded = true;
    editComp.groupId = groupId;
    editComp.taskId = taskId;
  }

  public async ChangedTaskDescription(taskId: string, newDescription: string) {
  const selectedTask = this.tasks.find(w => w.id === taskId);
  if (selectedTask !== undefined && selectedTask !== null) {
    if (selectedTask.description !== newDescription) { // Prevents Spam
      // Change Description in Database
      selectedTask.description = newDescription;
      await this.tasksService.editTaskDescription(taskId, newDescription).subscribe(result=> {
        this.toastrService.primary('Beschreibung wurde geändert.', 'Änderung');
      }, err => {
        console.log('ERRUR:731');
      });
    }
   }
  }

  public async ChangedTaskTitle(taskId: string, newTitle: string) {
    const selectedTask = this.tasks.find(w => w.id === taskId);
    if (selectedTask !== undefined && selectedTask !== null) {
      if (selectedTask.title !== newTitle) { // Prevents Spam
        // Change Description in Database
        selectedTask.title = newTitle;
        await this.tasksService.editTaskTitle(taskId, newTitle).subscribe(result => {
          this.toastrService.primary('Titel wurde geändert.', 'Änderung');
        }, err => {
          console.log('ERRUR:730');
        });
      }
     }
    }

  public async dropTaskToNewRow(event: CdkDragDrop<string[]>) {
    const taskId = event.item.data.id;
    let newStatus = Task.StatusEnum.NUMBER_0;
    let newRow = 0;
    // Add Status/Row change to database
    switch (event.container.id) {
      case 'openGroupTasks':
          newStatus = Task.StatusEnum.NUMBER_0;
          newRow = 0;
          break;
      case 'inProgressGroupTasks':
          newStatus = Task.StatusEnum.NUMBER_1;
          newRow = 1;
          break;
      case 'doneGroupTasks':
          newStatus = Task.StatusEnum.NUMBER_2;
          newRow = 2;
          break;
          default:
      console.log('ERRUR: 230');
      return;
    }

    transferArrayItem(event.previousContainer.data,
      event.container.data,
      event.previousIndex,
      event.currentIndex);

    await this.tasksService.moveTask(taskId, newStatus, newRow).subscribe(result => {
      // Already move that task in order to have a smooth user experience

    // Apply changes to board (Card already got moved... to look more smooth)
    }, err => {
      console.log('ERRUR:830');
      // Move Card Back cuz changes couldn't be applied to backend
      transferArrayItem(event.container.data,
        event.previousContainer.data,
        event.currentIndex,
        event.previousIndex);
    });
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
    const taskIndex = this.tasks.findIndex(w => w.id === id);

    if (taskIndex !== -1) {
      this.tasks.splice(taskIndex, 1);
    }
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
    }, err => {
      console.log(err);
    });

  }

  public async AddTaskToMyBoard(groupId, row) {

    if (groupId !== undefined && groupId != null && groupId !== 0) {
      let newTask: Task = {
        title: 'Titel',
         description: 'Beschreibung',
         status: row,
         type: Task.TypeEnum.NUMBER_1,
         ownerId: groupId
        };

      // Add to Database
      newTask = await this.tasksService.createTask(newTask).toPromise();
      this.tasks.push(newTask);

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
