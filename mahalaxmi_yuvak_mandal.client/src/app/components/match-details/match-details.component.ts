import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatchService } from '../../services/match.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-match-details',
  standalone: false,
  templateUrl: './match-details.component.html',
  styleUrl: './match-details.component.css'
})
export class MatchDetailsComponent {

  matchId!: number;
  match: any;

  scores: any[] = [];
  teams: any;
  scoreForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private scoreService: MatchService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {

    this.matchId = Number(this.route.snapshot.paramMap.get('id'));

    this.scoreForm = this.fb.group({
      teamName: ['', Validators.compose([Validators.required])],
      runs: ['', Validators.compose([Validators.required, Validators.minLength(0)])],
      wickets: ['', Validators.compose([Validators.required, Validators.maxLength(10)])],
      oversPlayed: ['', Validators.compose([Validators.required])]
    });
    this.getMatchDetails(this.matchId);
    this.getScores();

  }

  getMatchDetails(id: any) {

    this.matchService.getMatchById(id).subscribe((res: any) => {
      this.match = res;
      console.log("match details : ", this.match);
      this.teams = [res.TeamA, res.TeamB];
      this.scoreForm.get('oversPlayed')?.setValue(res.Overs);
    });

  }

  getScores() {
    this.scoreService.getScores(this.matchId).subscribe((res: any) => {
      this.scores = res;
    });
  }

  addScore() {
    if (this.scores.length >= 2) {
      alert("Both teams' scores have already been entered. Cannot add more.");
      return;
    }
    const formValue = this.scoreForm.value;
    const teamName = formValue.teamName;
    const runs = Number(formValue.runs);
    const wickets = Number(formValue.wickets);



    const requiredFields = [
      { key: 'teamName', label: 'Team Name' },
      { key: 'runs', label: 'Runs' },
      { key: 'wickets', label: 'Wickets' },
      { key: 'oversPlayed', label: 'Overs' }
    ];

    for (const field of requiredFields) {
      // FIX: Access the control value directly to ensure you get the latest state
      const controlValue = this.scoreForm.get(field.key)?.value;

      if (controlValue === null || controlValue === undefined || controlValue === '') {
        alert(`Please enter a valid value for ${field.label}.`);
        return;
      }
    }

    // Find the other team's score if it already exists in the table
    const otherTeamScore = this.scores.find(s => s.TeamName !== teamName);

    if (this.match) {
      // SCENARIO A: You are entering the LOSER'S score
      if (teamName !== this.match.WinnerTeam) {
        const winnerData = this.scores.find(s => s.TeamName === this.match.WinnerTeam);
        if (winnerData && runs >= winnerData.Runs) {
          alert(`Invalid! The losing team (${teamName}) must have FEWER runs than the winner.`);
          return;
        }
      }

      // SCENARIO B: You are entering the WINNER'S score
      if (teamName === this.match.WinnerTeam) {
        const loserData = this.scores.find(s => s.TeamName !== this.match.WinnerTeam);
        if (loserData && runs <= loserData.Runs) {
          alert(`Invalid! The winning team (${teamName}) must have MORE runs than the opponent.`);
          return;
        }
      }
    }

    // Proceed with API call...
    const data = {
      matchId: this.matchId,
      teamName: teamName,
      runs: runs,
      wickets: wickets,
      oversPlayed: parseFloat(formValue.oversPlayed)
    };

    this.scoreService.createScore(data).subscribe(() => {
      this.getScores();
      this.scoreForm.reset({ teamName: '', runs: 0, wickets: 0, oversPlayed: this.match.Overs });
    });
  }

  getWinMargin(): string {
    if (this.scores.length < 2) return '';

    const winner = this.scores.find(s => s.TeamName === this.match.WinnerTeam);
    const loser = this.scores.find(s => s.TeamName !== this.match.WinnerTeam);

    if (winner && loser) {
      const runDiff = winner.Runs - loser.Runs;
      return `${winner.TeamName} won by ${runDiff} runs`;
    }
    return '';
  }

  calculateTotalRuns(): number {
    return this.scores.reduce((total, s) => total + s.Runs, 0);
  }


}
