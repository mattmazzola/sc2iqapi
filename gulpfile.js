var gulp = require('gulp');
var $ = require('gulp-load-plugins')({lazy: true});

gulp.task('help', $.taskListing);
gulp.task('default', ['help']);

gulp.task('lint', function () {
  return gulp
    .src(['./src/**/*.js', './gulpfile.js'])
    .pipe($.eslint())
    // eslint.format() outputs the lint results to the console.
    // Alternatively use eslint.formatEach() (see Docs).
    .pipe($.eslint.format())
    // To have the process exit with an error code (1) on
    // lint error, return the stream and pipe to failAfterError last.
    .pipe($.eslint.failAfterError());
});

gulp.task('lint-watcher', function () {
  gulp.watch(['./src/**/*.js', './gulpfile.js'], ['lint']);
});
