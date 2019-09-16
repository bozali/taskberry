import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupsAddUserComponent } from './groups-add-user.component';

describe('GroupsAddUserComponent', () => {
  let component: GroupsAddUserComponent;
  let fixture: ComponentFixture<GroupsAddUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupsAddUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupsAddUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
