import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PlayerStsatsService } from '../../services/player-stsats.service';

@Component({
  selector: 'app-create-player-stats',
  standalone: false,
  templateUrl: './create-player-stats.component.html',
  styleUrls: ['./create-player-stats.component.css']
})
export class CreatePlayerStatsComponent implements OnInit {

  statsForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private statsService: PlayerStsatsService
  ) { }

  ngOnInit(): void {

    this.statsForm = this.fb.group({

      matchId: ['', Validators.required],
      playerName: ['', Validators.required],
      teamName: ['', Validators.required],

      runs: [0],
      ballsFaced: [0],
      fours: [0],
      sixes: [0],

      oversBowled: [0],
      runsConceded: [0],
      wickets: [0]

    });

  }

  saveStats() {

    if (this.statsForm.invalid) {
      return;
    }

    this.statsService.createPlayerStats(this.statsForm.value)
      .subscribe({
        next: (res) => {
          alert("Player stats saved successfully");
          this.statsForm.reset();
        },
        error: (err) => {
          console.log(err);
        }
      });

  }

}
