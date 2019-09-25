import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardGroupBoardsComponent } from './dashboard-group-boards.component';

describe('DashboardGroupBoardsComponent', () => {
  let component: DashboardGroupBoardsComponent;
  let fixture: ComponentFixture<DashboardGroupBoardsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardGroupBoardsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardGroupBoardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
