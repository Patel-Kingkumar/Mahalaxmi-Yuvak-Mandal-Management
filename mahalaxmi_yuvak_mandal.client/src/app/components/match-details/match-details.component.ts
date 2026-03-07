import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatchService } from '../../services/match.service';

@Component({
  selector: 'app-match-details',
  standalone: false,
  templateUrl: './match-details.component.html',
  styleUrl: './match-details.component.css'
})
export class MatchDetailsComponent {

  matchId!: number;
  match: any;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService
  ) { }

  ngOnInit() {

    this.matchId = Number(this.route.snapshot.paramMap.get('id'));

    this.getMatchDetails();

  }

  getMatchDetails() {

    this.matchService.getMatchById(this.matchId).subscribe((res: any) => {
      this.match = res;
      console.log("match details : ", this.match);
    });

  }

}
