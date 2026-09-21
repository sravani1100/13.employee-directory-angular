import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessRights } from './access-rights';

describe('AccessRights', () => {
  let component: AccessRights;
  let fixture: ComponentFixture<AccessRights>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccessRights],
    }).compileComponents();

    fixture = TestBed.createComponent(AccessRights);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
