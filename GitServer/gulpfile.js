"use strict";

// Load plugins
const gulp = require("gulp");
const browsersync = require("browser-sync").create(); // https://browsersync.io/docs/gulp
const del = require("del");	// https://www.npmjs.com/package/del
const newer = require("gulp-newer"); // https://www.npmjs.com/package/gulp-newer
const imagemin = require("gulp-imagemin"); // https://www.npmjs.com/package/gulp-imagemin
const concat = require('gulp-concat');
const cleanCSS = require('gulp-clean-css');
const uglify = require('gulp-uglify');

/*
const eslint = require("gulp-eslint");
const cssnano = require("cssnano");
const postcss = require("gulp-postcss");
const rename = require("gulp-rename");

const autoprefixer = require("autoprefixer");

const cp = require("child_process");
const plumber = require("gulp-plumber");
const sass = require("gulp-sass");
const webpack = require("webpack");
const webpackconfig = require("./webpack.config.js");
const webpackstream = require("webpack-stream");
*/

// BrowserSync
function browserSync(done) {
  browsersync.init({
    server: {
      baseDir: "./wwwroot/"
    },
    port: 3000
  });
  done();
}

// BrowserSync Reload
function browserSyncReload(done) {
  browsersync.reload();
  done();
}

// Clean assets
function clean() {
  return del(["./wwwroot/css/**/*", "./wwwroot/js/**/*", "./wwwroot/lib/**/*"]);
}

var app = {};

app.addExtLibAsIs = function(path, outputPath) {
	return gulp.src(path)
	.pipe(gulp.dest(outputPath));
}

/*
app.addStyle = function(paths, outputFilename) {
    return gulp.src(paths)
    .pipe(sourcemaps.init())
    .pipe(sass().on('error', sass.logError))
    .pipe(debug({title: 'sass:'}))
    .pipe(concat(outputFilename))
    .pipe(debug({title: 'concat:'}))
    .pipe(gulpif(argv.prod, cleanCSS()))
    .pipe(sourcemaps.write('.'))
    .pipe(gulp.dest('./wwwroot/css'));
};

/* 
app.addScript = function(paths, outputFilename) {
    return gulp.src(paths)
    .pipe(sourcemaps.init())
    .pipe(debug({title: 'js:'}))
    .pipe(concat(outputFilename))
    .pipe(debug({title: 'jsconcat:'}))
    .pipe(gulpif(argv.prod, uglify()))
    .pipe(sourcemaps.write('.'))
    .pipe(gulp.dest('./wwwroot/js'));
};
*/

// Optimize Images
function images() {
  return gulp
    .src("./assets/img/**/*")
    .pipe(newer("./wwwroot/img"))
    .pipe(
      imagemin([
        imagemin.gifsicle({ interlaced: true }),
        imagemin.jpegtran({ progressive: true }),
        imagemin.optipng({ optimizationLevel: 5 }),
        imagemin.svgo({
          plugins: [
            {
              removeViewBox: false,
              collapseGroups: true
            }
          ]
        })
      ])
    )
    .pipe(gulp.dest("./wwwroot/img"));
}

function extLibs() {
	app.addExtLibAsIs("./assets/lib/jquery/dist/**/*", "./wwwroot/lib/jquery/dist/");
	app.addExtLibAsIs("./assets/lib/semantic/dist/*", "./wwwroot/lib/semantic/dist/");
	app.addExtLibAsIs("./assets/lib/semantic/dist/themes/default/**/*", "./wwwroot/lib/semantic/dist/themes/default/");
	app.addExtLibAsIs("./assets/lib/highlightjs/*", "./wwwroot/lib/highlightjs/");	
	app.addExtLibAsIs("./assets/lib/highlightjs/styles/foundation.css", "./wwwroot/lib/highlightjs/styles/");	
	app.addExtLibAsIs("./assets/lib/highlightjs/styles/github.css", "./wwwroot/lib/highlightjs/styles/");	
}

function css() {
    return gulp.src(["./assets/css/**/*.css"])
    // .pipe(sourcemaps.init())
    // .pipe(sass().on('error', sass.logError))
    .pipe(concat('main.css'))
    .pipe(cleanCSS())
    //.pipe(sourcemaps.write('.'))
    .pipe(gulp.dest('./wwwroot/css'));
}

function js() {
    return gulp.src(["./assets/js/*.js"])
	.pipe(concat('site.js'))
	.pipe(uglify())
	.pipe(gulp.dest('./wwwroot/js'));
}

/*
// define complex tasks
const js = gulp.series(scriptsLint, scripts);
const watch = gulp.parallel(watchFiles, browserSync);
*/
const build = gulp.series(clean, gulp.parallel(extLibs, css, images, /*jekyll,*/ js));

// export tasks
exports.clean = clean;
exports.extLibs = extLibs;
exports.images = images;
exports.css = css;
exports.js = js;

exports.build = build;
exports.default = build;

/*
exports.jekyll = jekyll;
*/