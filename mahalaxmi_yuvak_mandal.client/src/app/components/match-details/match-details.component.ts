import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatchService } from '../../services/match.service';
import { FormBuilder, FormGroup } from '@angular/forms';

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
      teamName: [''],
      runs: [''],
      wickets: [''],
      oversPlayed: ['']
    });

    this.getMatchDetails();
    this.getScores();

  }

  getMatchDetails() {

    this.matchService.getMatchById(this.matchId).subscribe((res: any) => {
      this.match = res;
      console.log("match details : ", this.match);
    });

  }

  getScores() {

    this.scoreService.getScores(this.matchId).subscribe((res: any) => {
      this.scores = res;
    });

  }

  addScore() {

    const data = {
      matchId: this.matchId,
      teamName: this.scoreForm.value.teamName,
      runs: Number(this.scoreForm.value.runs),
      wickets: Number(this.scoreForm.value.wickets),
      oversPlayed: parseFloat(this.scoreForm.value.oversPlayed)
    };

    this.scoreService.createScore(data).subscribe(() => {
      this.getScores();
      this.scoreForm.reset();
    });

  }


}
