$projects = "dbqf.core", "Backends\dbqf.Sql", "dbqf.Serialization", "Frontends\dbqf.WinForms", "Frontends\dbqf.WPF"
foreach ($project in $projects)
{
	$path = [System.IO.Path]::GetFileName($project)
	$csproj = join-path -path $PSScriptRoot -childpath ..\lib\$project\$path.csproj
	nuget pack $csproj -IncludeReferencedProjects -Prop Configuration=Release -build
}