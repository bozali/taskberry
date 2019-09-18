import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardGroupBoardComponent } from './dashboard-group-board.component';

describe('DashboardGroupBoardComponent', () => {
  let component: DashboardGroupBoardComponent;
  let fixture: ComponentFixture<DashboardGroupBoardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardGroupBoardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardGroupBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
