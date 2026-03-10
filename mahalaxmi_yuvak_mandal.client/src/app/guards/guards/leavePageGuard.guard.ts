import { CanDeactivateFn } from '@angular/router';

export const leavePageGuard: CanDeactivateFn<any> = (component) => {
  // Guard triggers if a match has started (1 score) but isn't finished (< 2 scores)
  if (component.scores && component.scores.length >= 0 && component.scores.length < 2) {
    return confirm("The match scorecard is incomplete! If you leave, your progress will not be saved. Do you really want to leave?");
  }
  return true;
};
