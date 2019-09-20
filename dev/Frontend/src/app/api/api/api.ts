export * from './authentication.service';
import { AuthenticationService } from './authentication.service';
export * from './groups.service';
import { GroupsService } from './groups.service';
export * from './tasks.service';
import { TasksService } from './tasks.service';
export * from './users.service';
import { UsersService } from './users.service';
export const APIS = [AuthenticationService, GroupsService, TasksService, UsersService];
