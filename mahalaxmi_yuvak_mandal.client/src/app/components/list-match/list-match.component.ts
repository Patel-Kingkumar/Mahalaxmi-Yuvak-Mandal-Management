import { Component } from '@angular/core';
import { MatchService } from '../../services/match.service';

@Component({
  selector: 'app-list-match',
  standalone: false,
  templateUrl: './list-match.component.html',
  styleUrl: './list-match.component.css'
})
export class ListMatchComponent {

  matches: any[] = [];

  constructor(private matchService: MatchService) { }

  ngOnInit() {
    this.getMatches();
  }

  getMatches() {
    this.matchService.getAllMatches().subscribe((res: any) => {
      this.matches = res;
    });
  }

  deleteMatch(id: number) {

    if (confirm('Are you sure you want to delete this match?')) {

      this.matchService.deleteMatch(id).subscribe(() => {
        this.getMatches();
      });

    }

  }

}
