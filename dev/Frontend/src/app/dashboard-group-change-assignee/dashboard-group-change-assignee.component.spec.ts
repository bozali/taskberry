import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardGroupChangeAssigneeComponent } from './dashboard-group-change-assignee.component';

describe('DashboardGroupChangeAssigneeComponent', () => {
  let component: DashboardGroupChangeAssigneeComponent;
  let fixture: ComponentFixture<DashboardGroupChangeAssigneeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardGroupChangeAssigneeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardGroupChangeAssigneeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
