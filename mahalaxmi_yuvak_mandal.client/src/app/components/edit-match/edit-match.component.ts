import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatchService } from '../../services/match.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-match',
  standalone: false,
  templateUrl: './edit-match.component.html',
  styleUrl: './edit-match.component.css'
})
export class EditMatchComponent {

  matchForm!: FormGroup;
  matchId!: number;

  constructor(
    private fb: FormBuilder,
    private matchService: MatchService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) { }

  ngOnInit() {

    this.matchForm = this.fb.group({
      TeamA: ['', Validators.required],
      TeamB: ['', Validators.required],
      MatchDate: ['', Validators.required],
      GroundName: ['', Validators.required],
      Overs: ['', Validators.required],
      MatchType: ['', Validators.required],
      WinnerTeam: ['']
    });

    this.matchId = Number(this.route.snapshot.paramMap.get('id'));

    this.getMatchById();
  }

  getMatchById() {

    this.matchService.getMatchById(this.matchId).subscribe((res: any) => {

      this.matchForm.patchValue({
        TeamA: res.TeamA,
        TeamB: res.TeamB,
        MatchDate: res.MatchDate,
        GroundName: res.GroundName,
        Overs: res.Overs,
        MatchType: res.MatchType,
        WinnerTeam: res.WinnerTeam
      });

    });

  }

  updateMatch() {

    if (this.matchForm.invalid) {
      this.matchForm.markAllAsTouched();
      return;
    }

    this.matchService.updateMatch(this.matchId, this.matchForm.value).subscribe({

      next: () => {
        // this.toastr.success('Match Updated Successfully');
        this.router.navigate(['/list-match']);
      },

      error: (err) => {
        console.error(err);
      }

    });

  }


}