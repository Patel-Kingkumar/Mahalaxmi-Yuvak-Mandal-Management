import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatchService } from '../../services/match.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-match',
  standalone: false,
  templateUrl: './create-match.component.html',
  styleUrl: './create-match.component.css'
})
export class CreateMatchComponent {

  matchForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private matchService: MatchService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  ngOnInit() {

    this.matchForm = this.fb.group({
      matchDate: ['', Validators.required],
      groundName: ['', Validators.required],
      teamA: ['', Validators.required],
      teamB: ['', Validators.required],
      overs: ['', Validators.required],
      matchType: ['', Validators.required],
      winnerTeam: ['']
    });

  }

  createMatch() {

    if (this.matchForm.invalid) {
      this.matchForm.markAllAsTouched();
      return;
    }

    this.matchService.createMatch(this.matchForm.value).subscribe({

      next: () => {
        // this.toastr.success('Match Created Successfully');
        this.router.navigate(['/list-match']);
      },

      error: (err) => {
        console.error(err);
      }

    });

  }

}
